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
				id = params[:id]
				user = JSON.parse params[:user]
				if token.nil?
					render :status=>400,
		              :json=>{:message=>"The request must contain a token."}
		       		return
		       	end
				@user = User.find_by_authentication_token(token)

				unless @user.id == id.to_i
					render :status => 401,
						:json => { :message => "Access Denied. Check user Id"}
					return
				end

				if User.update(id, user)
					render json: {user: @user}
				else
					render :status => 400,
						:json => { :message => "Something went wrong with saving user changes."}
					return
				end
			end
		end
	end
end