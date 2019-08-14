module.exports = function(app, tools){

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

    app.post('/new/chat', (req, res) => {
        //create a new chat
        
        var sql = ``
        var params = []

        let chat_name = req.body.chat_name
        let user_id = req.body.user_id

        //add the chat to the main database
        sql = `INSERT INTO "chats"("name") VALUES (?)`
        params = [chat_name]

        db.main.run(sql, params, function(err){
            if (tools.no_err(err, req, res)) { 

                chat_id = this.lastID

                //create a table to store users in a chat
                sql = `CREATE TABLE "` + chat_id + `" (
                    "id"	INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                    "user_id"	INTEGER,
                    "time_joined"	DATETIME,
                    "role"	TEXT
                )`

                db.chat_users.run(sql, [], function(err){ 
                    if (tools.no_err(err, req, res)) { 

                        //add user to above table
                        sql = `INSERT INTO "` + chat_id + `"("user_id","time_joined","role") VALUES (?,?,?);`
                        params = [user_id, new Date(), 'owner']

                        db.chat_users.run(sql, params, function(err){ tools.no_err(err, req, res) })
                    }
                })

                //create a table to store the messages of the chat
                sql = `CREATE TABLE "` + chat_id + `" (
                    "id"	INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE,
                    "user_id"	INTEGER,
                    "time_submitted"    INTEGER,
                    "message"	TEXT 
                )`

                db.chat.run(sql, [], function(err){ tools.no_err(err, req, res) })

                //add chat to the user that created it chats list
                sql = `INSERT INTO "` + user_id + `"("chat") VALUES (?)`
                params = [chat_id]

                db.users_chats.run(sql, params, function(err){ tools.no_err(err, req, res) })
                
                tools.return(res, { id: chat_id })

            }
        });
    })
}