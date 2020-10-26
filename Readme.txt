mkdir -p d:/Dev/mongodata_api_bet
docker pull mongo
-e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=secret