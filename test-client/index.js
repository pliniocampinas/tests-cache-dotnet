const http = require('http')
const testApi = 'http://localhost:5083'
const awaitMilliseconds = (ms) => new Promise((resolve) => setTimeout(resolve, ms))

const requestName = (name, key) => {
  return new Promise((resolve, reject) => {
    http.get(`${testApi}/?name=${name}&cacheKey=${key}`, res => {
      if(res.statusCode != 200) {
        console.log('Status Code:', res.statusCode)
        reject(Error('Request status code:' + res.statusCode))
      }
  
      let data = []
      res.on('data', chunk => {
        data.push(chunk)
      });
  
      res.on('end', () => {
        const parsedData = JSON.parse(Buffer.concat(data).toString())
        resolve(parsedData)
      });
    }).on('error', err => {
      console.log('Error: ', err.message)
      reject(err)
    });
  });
}

const testNames = [
  'plinio',
  // 'maria',
  // 'jose',
  // 'daniel',
  // 'david',
  // 'matias',
  // 'judas',
  // 'tadeu',
]

const repeatCalls = 3000

const runMultipleRequests = async () => {
  
  for (i = 0; i < repeatCalls; i++) {
    for (const testName of testNames) {
      console.log('testName', testName)
      requestName(testName, 'a')
    }
    await awaitMilliseconds(10)
  }
}

runMultipleRequests()


