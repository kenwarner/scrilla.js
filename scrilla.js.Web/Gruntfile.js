module.exports = function (grunt) {

  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),

    watch: {
      bower: {
        files: ['Gruntfile.js'],
        tasks: ['bower:install'],
        options: {
          spawn: false
        }
      }
    },

    bower: {
      install: {
        options: {
          targetDir: './lib',
          layout: 'byComponent',
          install: true,
          verbose: false,
          cleanTargetDir: true,
          cleanBowerDir: false,
          bowerOptions: {}
        }
      }
    },

    devUpdate: {
      npm: {
        options: {
          updateType: 'report',
          packages: {
            devDependencies: true,
            dependencies: true
          }
        }
      }
    }

  });

  grunt.loadNpmTasks('grunt-bower-task');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-dev-update');

  grunt.registerTask('default', ['bower', 'devUpdate']);
};
