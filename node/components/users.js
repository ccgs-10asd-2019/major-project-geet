module.exports = function(app, tools){

    app.get('/users/:chat_id', (req, res) => {
        //returns users in a chat
    
        let sql = 'SELECT * FROM `' + req.params.chat_id + '`'
        console.log(sql)
    
        db.chat_users.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })

    app.get('/user/:user_id', (req, res) => {
        //returns a username from a user_id
    
        let sql = 'SELECT `username` FROM `users` WHERE "id"="' + req.params.user_id + '"'
        console.log(sql)

        db.main.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { 
                if (JSON.stringify(rows) != "[]") {
                    res.send(rows) 
                } else {
                    res.send([{"username":"[deleted]"}])
                }
                
            }
        })
    
    })

    app.post('/auth/register', (req, res) => {
        //register a new user

        let username = req.body.username

        //create new user record
        let sql = 'INSERT INTO "main"."users" ("username") VALUES (?)'
        console.log(sql)
        let params = [username]
        db.main.run(sql, params, function(err){
            if (tools.no_err(err, req, res)) { 
                let user_id = this.lastID

                //create table to store users chats
                sql = 'CREATE TABLE "' + user_id + '" ( "chat" INTEGER UNIQUE )'
                console.log(sql)
                
                db.users_chats.run(sql, [], function(err){ 
                    if (tools.no_err(err, req, res)) {
                        tools.return(res, { id: user_id  })
                    } 
                })
            }
        })
    })

    app.post('/auth/login', (req, res) => {
        //login

        let username = req.body.Username

        //get id of user
        sql = 'SELECT id FROM `users` WHERE "username"="' + username + '"'
        console.log(sql)
        db.main.get(sql, [], (err, result) => {
            if (tools.no_err(err, req, res)) { 
                if (result == undefined) 
                    { tools.return(res, { id: null }) } 
                else 
                    { tools.return(res, result) }
            }
        })
    })
}