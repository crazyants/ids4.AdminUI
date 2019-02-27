module.exports = {
  lintOnSave: false,
  devServer: {
    proxy: {
      '/connect': {
<<<<<<< Updated upstream
        target: 'http://192.168.103.251:6006'
      },
      '/base': {
        target: 'http://192.168.103.251:6006'
=======
        target: 'http://127.0.0.1:5000'
>>>>>>> Stashed changes
      }
    }
    // proxy: {
    //   '/connect': {
    //     target: 'http://localhost:5000'
    //   },
    //   '/base': {
    //     target: 'http://localhost:5000'
    //   }
    // }
  }
}