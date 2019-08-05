module.exports = function(app){

    app.get('/info/:info/:chat_id', (req, res) => {
        //returns name of chat
    
        let sql = 'SELECT `' + req.params.info + '` FROM `chats` WHERE "id"="' + req.params.chat_id + '"'

        db.main.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })

    app.get('/chats/:user_id', (req, res) => {
        //returns chats a user is in
    
        let sql = 'SELECT * FROM `' + req.params.user_id + '`'
        
        db.users_chats.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })

    app.get('/icon/:chat_id', (req, res) => {
        //returns a chats icon

        res.sendFile("server_data/" + req.params.chat_id + "/icon.png", { root: __dirname + '/../' })
    })
}