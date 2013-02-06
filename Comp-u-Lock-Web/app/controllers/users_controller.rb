class UsersController < ApplicationController
	load_and_authorize_resource
	before_filter :authenticate_user!#, :except => [:index, :list]
	def index
		@computers = current_user.computer
	end

	def list
		@users = User.all
	  	if can? :read, @users
	  		@users.reverse
	  	else
	  		flash[:notice] ="Sorry but you dont have rights to that page."
	  		redirect_to :action => "index"
	  	end
	end
end