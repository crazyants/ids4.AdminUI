module.exports = {
  lintOnSave: false,
  devServer: {
    proxy: {
      '/dmp': {
        target: 'http://172.16.0.148:20001'
      }
    }
  }
}