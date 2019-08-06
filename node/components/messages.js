module.exports = function(app){

    app.get('/messages/:chat_id', (req, res) => {
        //returns a list of all the messages in a chat
      
      let sql = 'SELECT * FROM `' + req.params.chat_id + '`'
    
      db.chats.all(sql, [], (err, rows) => {
        if (err) { res.send(err) } 
        else { res.send(rows) }
      })
    })
    
    app.post('/message', (req, res) => {
        //to recieve messages sent from client
      
      console.log(req.body)

      let sql = 'INSERT INTO "' + req.body.Chat_id + '"("user_id","time_submitted","message") VALUES (?,?,?);'
      let params = [req.body.User_id, req.body.Current_time, req.body.Message]

      db.chats.run(sql, params)
      res.status(200).send("ok")
    
    })
}