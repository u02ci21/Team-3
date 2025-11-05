const localStrategy = require('passport-local').Strategy
const bcrypt = require('bcrypt')


function initialize(passport, getUserByEmail, getUserById) {
    //Function to authenticate users
    const authenticateUser = async (email, password, done) => {

        getUserByEmail = (email, (error, user) => {
            if (error) 
                return done(error);
            if (!user)
                return done(null, false, { message: 'No user found with that email' });

            bycrypt.compare(password, user.password, (error, isMatch) => {
                if (error) 
                    return done(error);
                if (isMatch) 
                    return done(null, user);
                else 
                    return done(null, false, { message: 'Password incorrect' });
            });
        });
    }

    passport.use(new localStrategy({ usernameField: 'email' }, authenticateUser))
    passport.serializeUser((user, done) => done(null, user.id))
    passport.deserializeUser((id, done) => {
        return done(null, getUserById(id))
    })
}

module.exports = initialize