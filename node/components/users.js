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

    app.get('/auth/register', (req, res) => {
        //register a new user

        let username = "bob" //will become a post request that takes a username and probs password

        //create new user record
        let sql = 'INSERT INTO "main"."users" ("username") VALUES (?)'
        let values = [username]
        db.main.run(sql, values)

        //get id of new user
        sql = 'SELECT id FROM `users` WHERE "username"="' + username + '"'
        db.main.all(sql, [], (err, rows) => {

            //create table to store users chats
            sql = 'CREATE TABLE "' + rows[0].id + '" ( "chat" INTEGER UNIQUE )'
            db.users_chats.run(sql, [])

            res.send(rows)
        })
    })

    app.post('/auth/login', (req, res) => {
        //register a new user

        let username = req.body.Username

        //get id of new user
        sql = 'SELECT id FROM `users` WHERE "username"="' + username + '"'
        db.main.get(sql, [], (err, result) => {
            if (result == undefined) { res.send(null) } 
            else { res.send(String(result.id)) }
        })
    })
}