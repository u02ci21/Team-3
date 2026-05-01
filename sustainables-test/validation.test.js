// validation.test.js
// Run with: npx jest validation.test.js

function getAge(dob) {
    const today = new Date(), birth = new Date(dob);
    let age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) age--;
    return age;
}

function isValidEmail(email) {
    return email.includes('@') && email.includes('.');
}

function validateSignup(username, dob, email, password, termsChecked = true) {
    if (username.length < 3)  return { ok: false, msg: 'Username needs at least 3 letters.' };
    if (!dob)                 return { ok: false, msg: 'Please enter your date of birth.' };
    if (!isValidEmail(email)) return { ok: false, msg: 'Please enter a valid email address.' };
    if (password.length < 6)  return { ok: false, msg: 'Password needs at least 6 characters.' };
    if (!termsChecked)        return { ok: false, msg: 'You must agree to the Terms and Conditions.' };
    if (getAge(dob) < 13)     return { ok: false, under13: true, msg: 'Under 13 — parental consent required.' };
    return { ok: true };
}

function validateLogin(email, password) {
    if (!email)    return { ok: false, msg: 'Please type your email.' };
    if (!password) return { ok: false, msg: 'Please type your password.' };
    return { ok: true };
}

function validateParentEmail(parentEmail, childEmail, termsChecked = true) {
    if (!isValidEmail(parentEmail))      return { ok: false, msg: 'Please enter a valid email address.' };
    if (parentEmail === childEmail)      return { ok: false, msg: 'Please enter a different email to your own.' };
    if (!termsChecked)                   return { ok: false, msg: 'You must agree to the Terms and Conditions.' };
    return { ok: true };
}

function validateVerifyCode(entered, generated) {
    if (entered !== generated) return { ok: false, msg: 'That code is incorrect. Please try again.' };
    return { ok: true };
}

function validateForgotPassword(email) {
    if (!isValidEmail(email)) return { ok: false, msg: 'Please enter a valid email address.' };
    return { ok: true };
}

function validateResetPassword(newPassword, confirmPassword) {
    if (newPassword.length < 6)        return { ok: false, msg: 'Password needs at least 6 characters.' };
    if (newPassword !== confirmPassword) return { ok: false, msg: 'Passwords do not match.' };
    return { ok: true };
}

function dateYearsAgo(years) {
    const d = new Date();
    d.setFullYear(d.getFullYear() - years);
    return d.toISOString().split('T')[0];
}

// TESTS

describe('Sign Up — Username', () => {
    test('empty username fails', () => {
        expect(validateSignup('', '2000-01-01', 'a@b.com', 'password123'))
            .toEqual({ ok: false, msg: 'Username needs at least 3 letters.' });
    });

    test('username of 2 characters fails', () => {
        expect(validateSignup('ab', '2000-01-01', 'a@b.com', 'password123'))
            .toEqual({ ok: false, msg: 'Username needs at least 3 letters.' });
    });

    test('username of exactly 3 characters passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', 'password123').ok).toBe(true);
    });

    test('username of 10 characters passes', () => {
        expect(validateSignup('helloworld', '2000-01-01', 'a@b.com', 'password123').ok).toBe(true);
    });
});

describe('Sign Up — Date of Birth', () => {
    test('missing date of birth fails', () => {
        expect(validateSignup('abc', '', 'a@b.com', 'password123'))
            .toEqual({ ok: false, msg: 'Please enter your date of birth.' });
    });

    test('age exactly 13 passes', () => {
        expect(validateSignup('abc', dateYearsAgo(13), 'a@b.com', 'password123').ok).toBe(true);
    });

    test('age 14 passes', () => {
        expect(validateSignup('abc', dateYearsAgo(14), 'a@b.com', 'password123').ok).toBe(true);
    });

    test('age 12 triggers parental consent', () => {
        expect(validateSignup('abc', dateYearsAgo(12), 'a@b.com', 'password123'))
            .toEqual({ ok: false, under13: true, msg: 'Under 13 — parental consent required.' });
    });

    test('age 5 triggers parental consent', () => {
        expect(validateSignup('abc', dateYearsAgo(5), 'a@b.com', 'password123'))
            .toEqual({ ok: false, under13: true, msg: 'Under 13 — parental consent required.' });
    });

    test('age 0 triggers parental consent', () => {
        expect(validateSignup('abc', dateYearsAgo(0), 'a@b.com', 'password123'))
            .toEqual({ ok: false, under13: true, msg: 'Under 13 — parental consent required.' });
    });
});

describe('Sign Up — Email', () => {
    test('missing @ fails', () => {
        expect(validateSignup('abc', '2000-01-01', 'notanemail', 'password123'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('missing . fails', () => {
        expect(validateSignup('abc', '2000-01-01', 'test@noperiod', 'password123'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('empty email fails', () => {
        expect(validateSignup('abc', '2000-01-01', '', 'password123'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('valid email passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'test@example.com', 'password123').ok).toBe(true);
    });
});

describe('Sign Up — Password', () => {
    test('empty password fails', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', ''))
            .toEqual({ ok: false, msg: 'Password needs at least 6 characters.' });
    });

    test('password of 5 characters fails', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', '12345'))
            .toEqual({ ok: false, msg: 'Password needs at least 6 characters.' });
    });

    test('password of exactly 6 characters passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', '123456').ok).toBe(true);
    });

    test('password of 20 characters passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', 'averylongpassword123').ok).toBe(true);
    });
});

describe('Log In', () => {
    test('empty email fails', () => {
        expect(validateLogin('', 'password123'))
            .toEqual({ ok: false, msg: 'Please type your email.' });
    });

    test('empty password fails', () => {
        expect(validateLogin('a@b.com', ''))
            .toEqual({ ok: false, msg: 'Please type your password.' });
    });

    test('both fields filled passes', () => {
        expect(validateLogin('a@b.com', 'password123').ok).toBe(true);
    });
});

describe('Sign Up — Terms and Conditions', () => {
    test('unchecked terms fails', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', 'password123', false))
            .toEqual({ ok: false, msg: 'You must agree to the Terms and Conditions.' });
    });

    test('checked terms passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', 'password123', true).ok).toBe(true);
    });

    test('terms checked by default passes', () => {
        expect(validateSignup('abc', '2000-01-01', 'a@b.com', 'password123').ok).toBe(true);
    });
});

describe('Parental Consent — Terms and Conditions', () => {
    test('unchecked terms fails', () => {
        expect(validateParentEmail('parent@test.com', 'child@test.com', false))
            .toEqual({ ok: false, msg: 'You must agree to the Terms and Conditions.' });
    });

    test('checked terms passes', () => {
        expect(validateParentEmail('parent@test.com', 'child@test.com', true).ok).toBe(true);
    });
});


describe('Parental Consent — Parent Email', () => {
    test('invalid email fails', () => {
        expect(validateParentEmail('notanemail', 'child@test.com'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('same as child email fails', () => {
        expect(validateParentEmail('child@test.com', 'child@test.com'))
            .toEqual({ ok: false, msg: 'Please enter a different email to your own.' });
    });

    test('empty parent email fails', () => {
        expect(validateParentEmail('', 'child@test.com'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('different valid email passes', () => {
        expect(validateParentEmail('parent@test.com', 'child@test.com').ok).toBe(true);
    });

    test('same domain but different address passes', () => {
        expect(validateParentEmail('parent@test.com', 'child@test.com').ok).toBe(true);
    });
});

describe('Parental Consent — Verify Code', () => {
    test('correct code passes', () => {
        expect(validateVerifyCode('123456', '123456').ok).toBe(true);
    });

    test('wrong code fails', () => {
        expect(validateVerifyCode('000000', '123456'))
            .toEqual({ ok: false, msg: 'That code is incorrect. Please try again.' });
    });

    test('empty code fails', () => {
        expect(validateVerifyCode('', '123456'))
            .toEqual({ ok: false, msg: 'That code is incorrect. Please try again.' });
    });

    test('code with spaces fails', () => {
        expect(validateVerifyCode('123 456', '123456'))
            .toEqual({ ok: false, msg: 'That code is incorrect. Please try again.' });
    });
});

describe('Forgot Password', () => {
    test('invalid email fails', () => {
        expect(validateForgotPassword('notanemail'))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('empty email fails', () => {
        expect(validateForgotPassword(''))
            .toEqual({ ok: false, msg: 'Please enter a valid email address.' });
    });

    test('valid email passes', () => {
        expect(validateForgotPassword('test@example.com').ok).toBe(true);
    });
});

describe('Reset Password', () => {
    test('password too short fails', () => {
        expect(validateResetPassword('123', '123'))
            .toEqual({ ok: false, msg: 'Password needs at least 6 characters.' });
    });

    test('passwords do not match fails', () => {
        expect(validateResetPassword('password123', 'password456'))
            .toEqual({ ok: false, msg: 'Passwords do not match.' });
    });

    test('empty passwords fail', () => {
        expect(validateResetPassword('', ''))
            .toEqual({ ok: false, msg: 'Password needs at least 6 characters.' });
    });

    test('matching valid passwords pass', () => {
        expect(validateResetPassword('password123', 'password123').ok).toBe(true);
    });

    test('exactly 6 characters passes', () => {
        expect(validateResetPassword('123456', '123456').ok).toBe(true);
    });
});
