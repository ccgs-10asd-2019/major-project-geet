const sqlite3 = require('sqlite3')
const { validationResult } = require('express-validator');

module.exports = {
    connectToDB: function (DB) {
        return new sqlite3.Database('./db/' + DB + '.db', sqlite3.OPEN_READWRITE, (err) => { //create new db object and return it back to where it was called
            if (err) { console.error(err.message) } //log error to console  
            else { console.log('Connected to the ' + DB + ' database.'); } //log success of connecting to database
        });
    },
    no_err: function (err, req, res) {
        var errors = validationResult(req);
        if (!errors.isEmpty()) {
            console.log(new Date(), req.url, errors.array()) 
            let content = "[" + { errors: errors.array() } + "]"
            res.send([{ "task": false, "content": content }]) 
            return false
        }
        else if (err) {
            console.log(new Date(), req.url, err) 
            let content = "[" + JSON.stringify(err) + "]"
            res.send([{ "task": false, "content": content }]) 
            return false
        } else {
            return true
        }
    },
    return: function (res, result) {
        let content = "[" + JSON.stringify(result) + "]"
        res.send([{ "task": true, "content": content }]) 
    }
}