module Api
	module V1
		class UsersController  < ApplicationController
			before_filter :authenticate_user!
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
					format.json { render :json => {user: @user} }
				end

			end

			def update
				token = params[:auth_token]
				user = params[:user]
				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end

				@user = User.find_by_authentication_token(token)

				unless @user.id == user.id
					render :status => 401,
						:json => { :message => "Access Denied. Check user Id"}
					return
				end

				render :status => 400,
				:json => { :message => "update not implemented"}
				return
			end
		end
	end
end