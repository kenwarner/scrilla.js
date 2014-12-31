# any outdated npm packages?
npm outdated --depth=0

# install bower components
bower install

# build jqrangeslider
cd bower_components\jqrangeslider
npm install
grunt

# any outdated bower components?
cd ..\..\
bower list

# run grunt, which will copy bower components to \lib
grunt install