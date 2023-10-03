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
  'maria',
  'jose',
  'daniel',
  'david',
  'matias',
  'judas',
  'tadeu',
]

const repeatCalls = 300

const runMultipleRequests = async () => {
  
  for (i = 0; i < repeatCalls; i++) {
    for (const testName of testNames) {
      const res = await requestName(testName, '' + i)
      console.log('res', res?.name)
      await awaitMilliseconds(i > 20? 4500: 150)
    }
  }
}

runMultipleRequests()


