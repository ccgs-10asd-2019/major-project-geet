const sqlite3 = require('sqlite3')

module.exports = {
    connectToDB: function (DB) {
        return new sqlite3.Database('./db/' + DB + '.db', sqlite3.OPEN_READWRITE, (err) => { //create new db object and return it back to where it was called
            if (err) { 
                console.error(err.message) //log error to console
                db_fail = err.message //set db_fail to error, triggering "database is down"
            } 
            else { console.log('Connected to the ' + DB + ' database.'); } //log success of connecting to database
        });
    }
}