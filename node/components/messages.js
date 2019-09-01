module.exports = function(app, tools, check, upload, path, fs){

  app.get('/messages/:chat_id/since/:since', (req, res) => {
      //returns a list of all the messages in a chat
    
    let sql = 'SELECT * FROM `' + req.params.chat_id + '` WHERE "time_submitted">"' + req.params.since + '"'
  
    db.chat.all(sql, [], (err, rows) => {
      if (err) { res.send(err) } 
      else { res.send(rows) }
    })
  })
  
  app.post('/message', [
    check('Chat_id').isNumeric().escape(),
    check('User_id').isNumeric().escape(),
    check('Current_time').isNumeric().escape(),
    check('Message').escape(),
    ], (req, res) => {
    
    //to recieve messages sent from client
    
    if(tools.no_err(null, req, res)) {

      let sql = 'INSERT INTO "' + req.body.Chat_id + '"("user_id","time_submitted","message") VALUES (?,?,?);'
      let params = [req.body.User_id, req.body.Current_time, req.body.Message]

      db.chat.run(sql, params, function(err){ 
        if(tools.no_err(err, req, res)) {
          tools.return(res, { id: this.lastID  })
        }
      })
    }
  })

  app.get('/file/:chat_id/:file_id', (req, res) => {

    let sql = 'SELECT "file_name" FROM `' + req.params.chat_id + '` WHERE "file_id" = "' + req.params.file_id +'"'
    db.chat.get(sql, [], (err, result) => { 
      if(tools.no_err(err, req, res)) {
        res.download(path.join(__dirname, '../uploads/' + req.params.file_id), result.file_name)
      }
    })
  })

  app.post("/upload",
    upload.single("file"),
    (req, res) => {
      res.send(req.file.filename) 
    }
  );

  app.post('/addfiletochat', (req, res) => {

    if(tools.no_err(null, req, res)) {

      let sql = 'INSERT INTO "' + req.body.Chat_id + '"("user_id","time_submitted","file_id","file_name") VALUES (?,?,?,?);'
      let params = [req.body.User_id, req.body.Current_time, req.body.File_id, req.body.File_name]

      db.chat.run(sql, params, function(err){ 
        if(tools.no_err(err, req, res)) {
          tools.return(res, { id: this.lastID  })
        }
      })
    }

  })
}