db.auth('root', 'toor')

db = db.getSiblingDB('admin')

db.createUser({
    user: 'bet-api',
    pwd: 'bet-api-password',
    roles: [
        {
            role: 'readWrite',
            db: 'bets',
        },
    ],
})