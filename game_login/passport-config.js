const LocalStrategy = require('passport-local').Strategy
const bcrypt = require('bcrypt')

function initialize(passport, getUserByEmail, getUserById) {
    const authenticateUser = (email, password, done) => {
        getUserByEmail(email, async (error, user) => {
            if (error) 
                return done(error);
            if (!user) 
                return done(null, false, { message: 'No user found with that email' })

            try {
                const isMatch = await bcrypt.compare(password, user.password)
                if (isMatch) {
                    return done(null, user)
                } else {
                    return done(null, false, { message: 'Password incorrect' })
                }
            } catch (error) {
                return done(error)
            }
        })
    }

    passport.use(new LocalStrategy({ usernameField: 'email' }, authenticateUser))
    passport.serializeUser((user, done) => done(null, user.id))
    passport.deserializeUser((id, done) => {
        getUserById(id, (error, user) => {
            if (error) 
                return done(error)
            return done(null, user)
        })
    })
}

module.exports = initialize