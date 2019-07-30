const express = require('express')
const app = express()
const port = 3000 //server port
app.use(express.json()); //adds json functionality

const sqlite3 = require('sqlite3') //adds ability to manipulate sqlite db files
var db_fail = "";

console.log("Time: " + new Date()) //log to console time of server start
db = { //connect to db files
    "main": connectToDB('main'), //stores core lists, servers, users
    "chat": connectToDB('chats'), //stores all chat messages
    "chat_users": connectToDB('chat_users'), //stores users allowed to message in a chat
}

function connectToDB(DB) {
    return new sqlite3.Database('./db/' + DB + '.db', sqlite3.OPEN_READWRITE, (err) => { //create new db object and return it back to where it was called
        if (err) { 
            console.error(err.message) //log error to console
            db_fail = err.message //set db_fail to error, triggering "database is down"
        } 
        else { console.log('Connected to the ' + DB + ' database.'); } //log success of connecting to database
    });
}

app.get('/', (req, res) => {
    if(db_fail != "") { //if an error is being stored in db_fail because there was an error opening a db file
        res.write("can't connect to database(s)\n")
        res.write(db_fail) // error code
        res.end()
    }
    else {
        res.send("all g") //all is good
    }
})

app.get('/messages/:chat_id', (req, res) => {
    //returns a list of all the messages in a chat
  
  let sql = 'SELECT * FROM `' + req.params.chat_id + '`'

  db.chat.all(sql, [], (err, rows) => {
    if (err) {
      res.send(err)
    } else {
      res.send(rows)
    }
  })

})

app.post('/message/:chat_id', (req, res) => {
    //to recieve messages sent from client
  
  console.log(req.body)
  res.send("thanks")

})

app.get('/info/:info/:chat_id', (req, res) => {
    //returns name of chat
  
  let sql = 'SELECT `' + req.params.info + '` FROM `chats` WHERE "id"="' + req.params.chat_id + '"'

  db.main.all(sql, [], (err, rows) => {
    if (err) {
      res.send(err)
    } else {
      res.send(rows[0])
    }
  })

})

app.get('/users/:chat_id', (req, res) => {
    //returns users in a chat
  
    let sql = 'SELECT * FROM `' + req.params.chat_id + '`'
  
    db.chat_users.all(sql, [], (err, rows) => {
      if (err) {
        res.send(err)
      } else {
        res.send(rows)
      }
    })
  
})

app.get('/user/:user_id', (req, res) => {
    //returns a username from a user_id
  
    let sql = 'SELECT `username` FROM `users` WHERE "id"="' + req.params.user_id + '"'
    console.log(sql)
    db.main.all(sql, [], (err, rows) => {
      if (err) {
        res.send(err)
      } else {
        res.send(rows[0])
      }
    })
  
  })

app.get('/icon/:chat_id', (req, res) => {
    //returns a chats icon
  
  res.sendFile("server_data/" + req.params.chat_id + "/icon.png", { root: __dirname })

})

app.listen(port, () => console.log(`listening on port ${port}`));