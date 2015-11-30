var MongoClient = require('mongodb').MongoClient;
var assert = require('assert');
var ObjectId = require('mongodb').ObjectID;
var url ='mongodb://localhost:27017/ErgoDB';
var partPoints = 'partPoints';
var insertPoints = function(data, callback) {
       //data consistes userId and their points
       MongoClient.connect(url, function(err, db){
           assert.equal(null,err);
           //form points json
           db.collection(partPoints).insertOne( {
                 "userId" : data.userId,
                 "points" : data.points,
                 "date" : new Date().getTime(),
                  }
                , function(err, result) {
                               assert.equal(err, null);
                               console.log("Inserted a document into the partPoints collection.");
                               callback(result);
                          });
        });
};
exports.insertPoints = insertPoints
