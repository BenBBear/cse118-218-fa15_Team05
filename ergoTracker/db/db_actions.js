var ip = '52.89.152.186';
var url = 'mongodb://' + ip + ':27017/ErgoDB';
var partPoints = 'PartPoints';

var mongoose = require('mongoose');

mongoose.connect(url);
var PartPoints = mongoose.model('PartPoints', {
    userId: String,
    date: Date,
    score: Number,
    points: mongoose.Schema.Types.Mixed
});


function insertPoints(data, score, cb) {
    var p = new PartPoints({
        "userId": data.userId,
        "points": data.points,
        "score": score,
        "date": new Date().getTime()
    });
    p.save(function(err) {
        if (err)
            cb(err);
        else
            cb(null);
    });
}

function findByDate(from, to,userId, cb) {
    PartPoints.find( //query today up to tonight
        {
            "userId":userId,
            "created_on": {
                "$gte": from,
                "$lt": to
            }
        }, function(err, results){
            if(err)
                cb(err);
            else
                cb(null, results);
        });
}



exports.insertPoints = insertPoints;
exports.findByDate = findByDate;
