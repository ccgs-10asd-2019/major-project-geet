module.exports = function(app){

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
      res.status(200).send("ok")
    
    })
}