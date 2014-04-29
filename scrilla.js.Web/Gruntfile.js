module.exports = function(grunt) {

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
          targetDir: './bowerlib',
          layout: 'byComponent',
          install: true,
          verbose: false,
          cleanTargetDir: true,
          cleanBowerDir: false,
          bowerOptions: {}
        }
      }
    }

  });

  grunt.loadNpmTasks('grunt-bower-task');
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.registerTask('default', ['bower']);
};
