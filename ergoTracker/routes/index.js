var express = require('express');
var router = express.Router();
var avg_score = Math.round(Math.random() * 100)/100 // This is cached score
var num = 1;
/* GET diagnostic data. */
router.get('/get/diagnostic_data', function(req, res, next) {
  var fromDate = req.query.fromDate;
  var toDate = req.query.toDate;
  var userId = req.query.userId;;
  var date = new Date();
  var prevd = new Date();
  //assert for the data types
  if(typeof(fromDate) == 'undefined' || typeof(toDate) == 'undefined' || typeof(userId) == 'undefined'){
    res.status(500);
    res.send("One of [fromDate, toDate, userId undefined");
    return;
  }
  //calculate the yesterday's date
  prevd.setTime(date.getTime() - 1000*60*60*24);
  var score_arr = {'scores': [{'date':prevd.toDateString(), 'score':41.25},{'date':date.toDateString(), 'score': 65}]} 

  res.status(404);
  res.send(score_arr);
});

/* POST kinect data. */
router.post('/post/kinect_data', function(req, res, next) {
  
    var err = { "message": "Data Inserted into database",
                "code": "0"
              };
    //post is a passive call.
    //If there are some points missing 
    //then we donot bother about it
    var score = Math.round(Math.random()*100)/100;
    avg_score = ((avg_score*num++) + score)/num; //post incrementing number to calculate previous sum   
    var result = {"error": err,
       "score":score,
       "break":true,
       "avg_score":avg_score.toFixed(2), //so far today
  }
  res.status(200);
  res.send(result);
});

/* GET today's score. */
router.get('/get/today_score', function(req, res, next) {
  var date = new Date()
  var score = Math.round(Math.random()*100)/100
  var userId = req.query.userId;
  if (typeof(userId) == "undefined"){
      res.status(500);
      res.send("userId not sent");
      return;
  }
  res.status(200);
  res.send(score.toFixed(2))
});
/* GET home page. */
router.get('/', function(req, res, next) {
  res.render('index', { title: 'Express' });
});
module.exports = router;
