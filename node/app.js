//consts / imports
const express = require('express')
const app = express()
const port = 3000 //server port
app.use(express.json()); //adds json functionality

const sqlite3 = require('sqlite3') //adds ability to manipulate sqlite db files
const tools = require('./components/tools')

//setup
console.log("Time: " + new Date()) //log to console time of server start
db = { //connect to db files
    "main": tools.connectToDB('main'), //stores core lists, servers, users
    "chat": tools.connectToDB('chats'), //stores all chat messages
    "chat_users": tools.connectToDB('chat_users'), //stores users allowed to message in a chat
    "users_chats": tools.connectToDB('users_chats'), //stores all the chats a user is in
}

//gets and posts
app.get('/', (req, res) => {
    res.send("all g") //all is good
})

require('./components/messages')(app, tools);
require('./components/chats')(app, tools);
require('./components/users')(app, tools);

app.listen(port, () => console.log(`listening on port ${port}`));