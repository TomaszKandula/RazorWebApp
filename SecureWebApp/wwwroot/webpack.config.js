const webpack = require('webpack');
const path = require('path');

const config = 
{
  mode: 'development',
  entry: 
	{
        bundle: 
		[
            './styles/site.scss',
            './scripts/site.js'
        ]
    },
  
  output: 
  {
    path: path.resolve(__dirname, 'dist'),
	//filename: '[name].[contenthash].js' 
	filename: '[name].js' 
  },
  
  module: 
  {
    rules: 
	[
      {
        test: /\.scss$/,
        use: 
		[
          'style-loader',
          'css-loader',
          'sass-loader'
        ]
      },
      {
        test: /\.png$/,
        use: 
		[
          {
            loader: 'url-loader',
            options: 
			{
              mimetype: 'image/png'
            }
          }
        ]
      },
      {
        test: /\.svg$/,
        use: 'file-loader'
      }
    ]
  }, 
  
  optimization: 
  {
    runtimeChunk: 'single',
    splitChunks: 
	{
      cacheGroups: 
	  {
        vendor: 
		{
          test: /[\\\/]node_modules[\\\/]/,
          name: 'vendors',
          chunks: 'all'
        }
      }
    }
  }
}

module.exports = config;
