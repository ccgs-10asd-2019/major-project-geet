module.exports = function (app, tools, crypto) {

    app.get('/users/:chat_id', (req, res) => {
        //returns users in a chat

        let sql = 'SELECT * FROM `' + req.params.chat_id + '`'
        console.log(sql)

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
        console.log(sql)

        db.main.all(sql, [], (err, rows) => {
            if (err) {
                res.send(err)
            } else {
                if (JSON.stringify(rows) != "[]") {
                    res.send(rows)
                } else {
                    res.send([{
                        "username": "[deleted]"
                    }])
                }

            }
        })

    })

    app.post('/auth/register', (req, res) => {
        //register a new user

        const username = req.body.Username
        const pass = saltHashPassword(req.body.Password);

        if (!(username && pass)) {
            tools.return(res, {
                id: null
            })
        }

        //create new user record
        let sql = 'INSERT INTO "main"."users" ("username", "password", "salt") VALUES (?, ?, ?)'
        console.log(sql)
        let params = [username, pass.password, pass.salt]
        db.main.run(sql, params, function (err) {
            if (tools.no_err(err, req, res)) {
                let user_id = this.lastID

                //create table to store users chats
                sql = 'CREATE TABLE "' + user_id + '" ( "chat" INTEGER UNIQUE )'
                console.log(sql)

                db.users_chats.run(sql, [], function (err) {
                    if (tools.no_err(err, req, res)) {
                        tools.return(res, {
                            id: user_id
                        })
                    }
                })
            }
        })
    })
    app.post('/auth/login', (req, res) => {
        //login

        const username = req.body.Username
        const password = req.body.Password

        if (!(username && password)) {
            tools.return(res, {
                id: null
            })
        }

        //get id of user
        sql = 'SELECT * FROM `users` WHERE "username"="' + username + '"'
        console.log(sql)
        db.main.get(sql, [], (err, result) => {
            if (tools.no_err(err, req, res)) {
                if (result == undefined) {
                    tools.return(res, {
                        id: null
                    })
                } else {
                    let sha = sha512(password, result.salt)
                    if (sha.password === result.password) {
                        let id = result.id
                        tools.return(res, {id})
                    } else {
                        tools.return(res, {
                            id: null
                        })
                    }
                    
                }
            }
        })
    })

    function genRandomString(length) {
        return crypto.randomBytes(Math.ceil(length / 2))
            .toString('hex') /** convert to hexadecimal format */
            .slice(0, length); /** return required number of characters */
    };

    function sha512(password, salt) {
        var hash = crypto.createHmac('sha512', salt); /** Hashing algorithm sha512 */
        hash.update(password);
        var value = hash.digest('hex');
        return {
            salt: salt,
            password: value
        };
    };

    function saltHashPassword(userpassword) {
        var salt = genRandomString(16); /** Gives us salt of length 16 */
        var passwordData = sha512(userpassword, salt);
        return (passwordData)
    }

}