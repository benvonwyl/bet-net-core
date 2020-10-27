## Fonctionnement
```
Post:Bet+---+
Get:Bet+----|
HealthCheck++
            |
            |
API+BET     |                                                       +---DOCKER------+-
+-----------+---+                                                   |-----------+   |
|               |                                                   || API|BET  |   |
| Controller    |                                                   +-----------+   |
+---------------+                                                   |               |
|               |                                                   +---------+     |
| Ser^ices      |                                                   || MongoDb|     |
+---------------+                                                   +---------+     |
|         Http  |                                                   |               |
| Dal     Client|                                                   +---------------+
+--+--------+---+                                                   || Mongo Express|
   |        |                                                       +---------------+
   |        |
   v        v
MongoDb  OfferAPI
+------+ +------+
|      | |      |
|bets  | |Offers|
|      | |      |
+------+ +------+

```

## Installation

needs an operating docker

- Clone the repository 
- Ensure that : docker > settings > files sharing contains this repository 
- Run : 

```bash
cd API-BET

# build/run
docker-compose up --build  -d #( run )

# stop + unmount images 
docker-compose rm -s -f 
```

## Usage

The API works with a mongodatabase and mongo express server. 

- mongodb will starts on port 27017 and can be explored with mongo-express on http://localhost:8081/
- .NETCore BET-API will launch on http://localhost:5000/ and / displays swagger.

Place a bet :  
```curl --location --request POST 'http://localhost:5000/bet' \
--header 'Content-Type: application/json' \
--data-raw '{
    "offers": [{"offerId": 506992501,  "odd": 1.22 }],
    "customerId": "00000000-0000-0000-0000-000000000000",
    "stake": 100
}'
```
- offerId and odd must be realistics.  
- customerId can be random since there are no validations.

Get a Bet : 
```
curl --location --request GET 'http://localhost:5000/bet/5f97592b781c22ea8b3e7ddd'
```
HealthCheck :  
```
curl --location --request GET 'http://localhost:5000/health'
```

## TODO
Enrich Swagger
Get Tests ... 

Split differents layer in different project, and harder: make it works with docker
Gherkin integration tests,
Use Db Transctions in case of real money transaction on user accounts, use transactions to avoid somebody creating two bets with the same money 
Authentication
Realistic liveness + readyness 
...

