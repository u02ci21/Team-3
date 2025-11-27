//back end
const mysql = require('mysql2');

//create a new MySQL connection using the mysql2 package
const connection = mysql.createConnection({
    host: 'localhost', 
    user: 'root',
    password: 'Charlie321@',
    database: 'game_db' });

//connect to the database
connection.connect((error) => {
    if (error) {
        console.error('Error connecting to the database:', error);
    } else {
        console.log('Connected to the MySQL database.');
    }
});



if (process.env.NODE_ENV !== 'production') {
    require('dotenv').config() //loading environment variables from a .env file into process.env 
}

// importing libraries that we installed using npm
const express = require('express')
const app = express()
app.set('view engine', 'ejs')
const bcrypt = require('bcrypt') // importing bcrypt package
const passport = require('passport')
const initializePassport = require('./passport-config')
const flash = require('express-flash')
const session = require('express-session')
const methodOverride = require('method-override')


initializePassport(
    passport, 

    // getUserByEmail
    (email, done) => {
        connection.query('SELECT * FROM users WHERE email = ?', [email], (error, results) => {
            if (error) 
                return done(error);
            if (results.length === 0) 
                return done(null, null);
            return done(null, results[0]);
        });
    },
    // getUserById
    (id, done) => {
        connection.query('SELECT * FROM users WHERE id = ?', [id], (error, results) => {
            if (error) 
                return done(error);
            if (results.length === 0) 
                return done(null, null);
            return done(null, results[0]);
        });
    })
 

app.use(express.urlencoded({ extended: false }))
app.use(flash())
app.use(session({
    secret: process.env.SECRET_KEY,
    resave: false, //dont save session if nothing is changed
    saveUninitialized: false //dont create session until something is stored
}))

app.use(passport.initialize())
app.use(passport.session())
app.use(methodOverride('_method'))


app.post('/login', checkNotAuthenticated, passport.authenticate('local', {
    successRedirect: '/',
    failureRedirect: '/login',
    failureFlash: true //to display error messages (like no user found with that email)
}
))


// configuring the register route to add new users 
app.post('/register',  checkNotAuthenticated, async (req, res) => {
    const {username,email, password} = req.body;

    //hash the password
    bcrypt.hash(password, 10, (error, hashedPassword) => {
        if (error) {
            console.error('Error hashing password:', error);
            return res.redirect('/register');
        }

        // Insert the new user into the database
        const query = 'INSERT INTO users (username, email, password) VALUES (?, ?, ?)';
        connection.query(query, [username, email, hashedPassword], (error, results) => {
            if (error) {
                if (error.code === 'ER_DUP_ENTRY') {
                    console.error('Error: Email already in use.');                    
                } else {
                    console.error('Error inserting user into database:', error);
                }
                console.log('Unsuccessful registration', results);
                return res.redirect('/register');        
            }

            console.log('New user registered:', results);
            res.redirect('/login'); // Redirect to login page after successful registration
        });
    });
    })
    

// Routes 
app.get('/', checkAuthenticated, (req, res) => {
    res.render('index.ejs', {name: req.user.name})
})

app.get('/login', checkNotAuthenticated, (req, res) => {
    res.render('login.ejs')
})

app.get('/register', checkNotAuthenticated, (req, res) => {
    res.render('register.ejs')
})
//End Routes

app.delete('/logout', (req, res, next) => {
    req.logOut(req.user, error => {
        if (error) {
            return next(error)
            res.redirect('/login')   
        }
    })
    
})


function checkAuthenticated(req, res, next) {
    if (req.isAuthenticated()) {
        return next()
    }
    res.redirect('/login')
}

function checkNotAuthenticated(req, res, next) {
    if (req.isAuthenticated()) {
        return res.redirect('/')
    }
    next() 
}

//close the MySQL connection 
// connection.end();


app.listen(3000)


 