var express = require('express');
var router = express.Router();
var db = require('../db/db_actions.js');

var bodyParser = require('body-parser');
// parse application/x-www-form-urlencoded 
router.use(bodyParser.urlencoded({
    extended: false
}));

// parse application/json 
router.use(bodyParser.json());


/* GET diagnostic data. */
router.get('/get/diagnostic_data', function(req, res, next) {
    var fromDate = req.query.fromDate;
    var toDate = req.query.toDate;
    var userId = req.query.userId;;
    var date = new Date();
    var prevd = new Date();
    //assert for the data types
    if (typeof(fromDate) == 'undefined' || typeof(toDate) == 'undefined' || typeof(userId) == 'undefined') {
        res.status(500);
        res.send("One of [fromDate, toDate, userId undefined");
        return;
    }
    //calculate the yesterday's date
    prevd.setTime(date.getTime() - 1000 * 60 * 60 * 24);
    var score_arr = {
        'scores': [{
            'date': prevd.toDateString(),
            'score': 41.25
        }, {
            'date': date.toDateString(),
            'score': 65
        }]
    }

    res.status(404);
    res.send(score_arr);
});

/* POST kinect data. */

var algorithm = require('2015-cse218-group5-algorithm');
var cache = require('memory-cache');
algorithm.FrameRate(5);



router.post('/post/kinect_data', function(req, res, next) {    
    var num, avg_score,result;
    algorithm(req.body.points, function(err, score, isBreak) {
        if (cache.get(req.body.userId)) {
            var t = cache.get(req.body.userId);
            num = t.num;
            avg_score = t.avg_score;
            avg_score = ((avg_score * num++) + score) / num;
        } else {
            cache.put(req.body.userId,{
                num:1,
                avg_score:score
            }, 1000*60*60);
            avg_score = score;
        }
        result = {
            "error": null,
            "score": score,
            "break": isBreak,
            "avg_score": avg_score.toFixed(2) //so far today
        };        
        db.insertPoints(req.body, score , function(err) {
            if (err)
                res.json({
                    "message": "Datebase Error",
                    "code": 1
                });
            else {                
                res.json(result);
            }
        });        
    });        
});


/* GET today's score. */
router.get('/get/today_score', function(req, res, next) {
    var date = new Date()
    var score = Math.round(Math.random() * 100) / 100
    var userId = req.query.userId;
    if (typeof(userId) == "undefined") {
        res.status(500);
        res.send("userId not sent");
        return;
    }
    res.status(200);
    res.send(score.toFixed(2))
});
/* GET home page. */
router.get('/', function(req, res, next) {
    res.render('index', {
        title: 'Express'
    });
});
module.exports = router;
