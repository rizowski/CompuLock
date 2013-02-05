class Api::V1::UsersController  < ApplicationController
	respond_to :json

	def index 
		token = params[:auth_token]
		if token.nil?
			render :status => 400,
				:json => { :message => "The request must contain an auth token."}
			return
		end
		@user = User.find_by_authentication_token(token)
		respond_to do |format|
			format.json { render :json => @user}
		end

	end

	def show
		token = params[:auth_token]

		if token.nil?
			render :status=>400,
              :json=>{ :message => "The request must contain an auth token."}
       		return
       	end

		@user = User.find_by_authentication_token(token)

       	respond_to do |format|
			format.json { render :json => @user}
		end
	end

	def edit
		token = params[:auth_token]

		if token.nil?
			render :status=>400,
              :json=>{ :message => "The request must contain an auth token."}
       		return
       	end

		@user = User.find_by_authentication_token(token)

       	respond_to do |format|
			format.json { render :json => @user}
		end
	end

	def update
		render :status => 400,
		:json => { :message => "update not implemented"}
		return
	end
end