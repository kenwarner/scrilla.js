// Karma configuration file

module.exports = function (config) {
  config.set({

    basePath: '../../',

    files: [
      'lib/angular/*.js',
      'app/**/*.js'
    ],

    frameworks: ['jasmine'],

    reporters: ['progress', 'coverage'],

    plugins: ['karma-jasmine', 'karma-phantomjs-launcher', 'karma-coverage'],

    browsers: ['PhantomJS'],

    colors: true,

    singleRun: false
  });
};