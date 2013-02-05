class Api::V1::UsersController  < ApplicationController
	respond_to :json

	def index 

	end
	def show
		token = params[:auth_token]

		if token.nil?
			render :status=>400,
              :json=>{ :message => "The request must contain an auth token."}
       		return
       	end

		@user = User.find_by_authentication_token(token)

		if @user.nil?
			render :status=>400,
              :json=>{ :message => "The server couldn't find a user by that token."}
       		return
       	end

       	render :status => 200, :json => @user
       	return
	end

	def edit
		token = params[:auth_token]

		if token.nil?
			render :status=>400,
              :json=>{ :message => "The request must contain an auth token."}
       		return
       	end

		@user = User.find_by_authentication_token(token)

		if @user.nil?
			render :status=>400,
              :json=>{ :message => "The server couldn't find a user by that token."}
       		return
       	end

       	render :status => 200, :json => {:user => @user}
       	return
	end

	def update
		render :status => 400,
		:json => { :message => "update not implemented"}
		return
	end
end