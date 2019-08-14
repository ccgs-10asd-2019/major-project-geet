module.exports = function(app){

    app.get('/users/:chat_id', (req, res) => {
        //returns users in a chat
    
        let sql = 'SELECT * FROM `' + req.params.chat_id + '`'
    
        db.chat_users.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })

    app.get('/user/:user_id', (req, res) => {
        //returns a username from a user_id
    
        let sql = 'SELECT `username` FROM `users` WHERE "id"="' + req.params.user_id + '"'

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
        let params = [username]
        db.main.run(sql, params, function(err){
            if (err) { 
                res.send([{ "task": false, "content": err }]) 
            } else {
                let user_id = this.lastID

                //create table to store users chats
                sql = 'CREATE TABLE "' + user_id + '" ( "chat" INTEGER UNIQUE )'
                db.users_chats.run(sql, [], function(err){ if (err) { res.send([{ "task": false, "content": err }]) }})

                let content = '[{ "id": ' + user_id + ' }]'
                res.send([{ "task": true, "content": content }])
            }
        })
    })

    app.post('/auth/login', (req, res) => {
        //login

        let username = req.body.Username

        //get id of user
        sql = 'SELECT id FROM `users` WHERE "username"="' + username + '"'
        db.main.get(sql, [], (err, result) => {
            if (err) { 
                res.send([{ "task": false, "content": err }])  
            }
            else if (result == undefined) 
            { 
                let content = '[{ "id": null }]'
                res.send([{ "task": true, "content": content }]) 
            } 
            else 
            { 
                let content = "[" + JSON.stringify(result) + "]"
                res.send([{ "task": true, "content": content }]) 
            }
        })
    })
}