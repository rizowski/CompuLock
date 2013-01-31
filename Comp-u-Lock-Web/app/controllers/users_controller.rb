class UsersController < ApplicationController
	before_filter :authenticate_user!#, :except => [:index, :list]
	def index
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

	def update
	end

	def save
	end
end