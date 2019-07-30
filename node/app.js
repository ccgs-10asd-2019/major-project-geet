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

console.log("Time: " + new Date())
main_db = connectToDB('main')
chats_db = connectToDB('chats')

app.get('/', (req, res) => {
  res.send("hey")
})

app.get('/messages/:chat_id', (req, res) => {
  
  let sql = 'SELECT * FROM `' + req.params.chat_id + '`'

  chats_db.all(sql, [], (err, rows) => {
    if (err) {
      res.send(err)
    } else {
      res.send(rows)
    }
  })

})

app.post('/message/:chat_id', (req, res) => {
  
  console.log(req.body)
  res.send("thanks")

})

app.get('/info/:info/:chat_id', (req, res) => {
  
  let sql = 'SELECT `' + req.params.info + '` FROM `chats` WHERE "id"="' + req.params.chat_id + '"'

  main_db.all(sql, [], (err, rows) => {
    if (err) {
      res.send(err)
    } else {
      res.send(rows[0])
    }
  })

})

app.get('/icon/:chat_id', (req, res) => {
  
  res.sendFile("server_data/" + req.params.chat_id + "/icon.png", { root: __dirname })

})

app.listen(port, () => console.log(`listening on port ${port}`));