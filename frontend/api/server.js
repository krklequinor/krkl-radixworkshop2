const http = require('http');

const server = http.createServer((req, res) => {
    let body = [];

    req.on('data', (chunk) => {
        body.push(chunk);
    });

    req.on('end', () => {
        body = Buffer.concat(body).toString();
        console.log(`Received body data: ${body}`);
        res.end('Received your request');
    });
});

server.listen(8030, () => {
    console.log('Server is listening on port 8030');
});