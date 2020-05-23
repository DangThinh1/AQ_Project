const http = require('http');
const fs = require('fs');

const hostname = '127.0.0.1';
const port = 3001;


//create server nodeJS
const server = http.createServer((req, res) => {
  res.statusCode = 200;
  res.setHeader('Content-Type', 'text/plain');
  res.setHeader("Access-Control-Allow-Origin", "*");
});

//get data
// app.post('/email', function(req, res){
// 	var obj = {};
// 	console.log('body: ' + JSON.stringify(req.body));
// 	res.send(req.body);
// });

// server.on('request', (request, response) => {
//   console.log(request)
// });

server.get("/url", (req, res, next) => {
  res.json(["Tony","Lisa","Michael","Ginger","Food"]);
});

//write data to json
let path = "emailList.json";
let obj = {
  name: "name",
  caption: "text"
}
let data = JSON.stringify(obj);
fs.writeFile(path, data, (err) => {
  if (err) throw err;
  // console.log('Data written to file');
});

server.listen(port, hostname, () => {
  console.log(`Server running at http://${hostname}:${port}/`);
});