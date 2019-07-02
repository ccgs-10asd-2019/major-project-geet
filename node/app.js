const express = require('express')
const app = express()
const port = 3000
app.use(express.json());

const sqlite3 = require('sqlite3')

function connectToDB(DB) {
  return new sqlite3.Database('./db/'+DB+'.db', sqlite3.OPEN_READWRITE, (err) => {
    if (err) { console.error(err.message); } 
    else { console.log('Connected to the '+DB+' database.'); }
  });
}

main_db = connectToDB('main')
chats_db = connectToDB('chats')

app.get('/getmessages/:chat_id', (req, res) => {
  
  let sql = 'SELECT * FROM `' + req.params.chat_id + '`'

  chats_db.all(sql, [], (err, rows) => {
    if (err) {
      res.send(null)
    } else {
      res.send(rows)
    }
  })

})

app.listen(port, () => console.log(`listening on port ${port}`))