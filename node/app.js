//consts / imports
const express = require('express')
const app = express()
const port = 3000 //server port
app.use(express.json()); //adds json functionality
const multer = require('multer');
const upload = multer({
    dest: "./uploads",
    limits: {
        fileSize: 10 * 1024 * 1024,
    },
  });
const { check } = require('express-validator');
const tools = require('./components/tools')
const path = require("path");
const fs = require("fs");

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

require('./components/messages')(app, tools, check, upload, path, fs);
require('./components/chats')(app, tools);
require('./components/users')(app, tools);

app.listen(port, () => console.log(`listening on port ${port}`));