module.exports = function(app){

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
            res.send(rows)
        }
        })
    
    })

}