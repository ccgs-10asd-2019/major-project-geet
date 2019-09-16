module.exports = function(app, tools){

    app.get('/collab/full/:chat_id/:id', (req, res) => {
        //returns text of chat
    
        let sql = 'SELECT collab FROM "' + req.params.chat_id + '" WHERE "id" = "' + req.params.id + '"'
        console.log(sql)

        db.chat.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })

    app.post('/collab/:line/:chat_id/:id', (req, res) => {
    
        console.log(req.body.Collab)
        console.log(req.body.Chat_id)
        console.log(req.body.Id)

        tools.return(res, { true: true })
    })

    app.get('/collab/last-edited/:chat_id/:id', (req, res) => {
        //returns time of last edit

        let sql = 'SELECT collab_lastedit FROM "' + req.params.chat_id + '" WHERE "id" = "' + req.params.id + '"'
        console.log(sql)

        db.chat.all(sql, [], (err, rows) => {
            if (err) { res.send(err) } 
            else { res.send(rows) }
        })
    })
}