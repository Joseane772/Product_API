#!/bin/bash
echo "create the table"
awslocal dynamodb create-table --table-name Products --attribute-definitions AttributeName=ID,AttributeType=S --key-schema AttributeName=ID,KeyType=HASH --provisioned-throughput ReadCapacityUnits=5,WriteCapacityUnits=5
echo "insert data"
awslocal dynamodb put-item --table-name Products --item '{"ID": {"S": "1"}, "Name": {"S": "Product 1"}, "Price": {"N": "100"}, "Description": {"S": "Description 1"}, "Stock": {"N": "10"}}'
awslocal dynamodb put-item --table-name Products --item '{"ID": {"S": "2"}, "Name": {"S": "Product 2"}, "Price": {"N": "200"}, "Description": {"S": "Description 2"}, "Stock": {"N": "20"}}'
