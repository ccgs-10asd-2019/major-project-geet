module.exports = function(app){

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
      
        res.sendFile("server_data/" + req.params.chat_id + "/icon.png", { root: __dirname + '/../' })
    
    })

}