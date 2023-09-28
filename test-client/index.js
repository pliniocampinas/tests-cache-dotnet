const http = require('http');

const requestName = (name) => {
  return new Promise((resolve, reject) => {
    http.get('http://localhost:5083/?name='+name, res => {
      console.log('Status Code:', res.statusCode);
  
      let data = [];
      res.on('data', chunk => {
        data.push(chunk);
      });
  
      res.on('end', () => {
        console.log('Response ended: ');
        const parsedData = JSON.parse(Buffer.concat(data).toString());
        resolve(parsedData);
      });
    }).on('error', err => {
      console.log('Error: ', err.message);
      reject(err);
    });
  });
}

const testNames = [
  'plinio',
  'maria',
  'jose',
  'daniel',
  'david',
]

const repeatCalls = 10

const awaitMilliseconds = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

const runMultipleRequests = async () => {
  
  for (const testName of testNames) {
    console.log('testName', testName)
    for (i = 0; i < repeatCalls; i++) {
      const res = await requestName(testName)
      console.log('res', res)
      await awaitMilliseconds(200)
    }
  }
}

runMultipleRequests()


