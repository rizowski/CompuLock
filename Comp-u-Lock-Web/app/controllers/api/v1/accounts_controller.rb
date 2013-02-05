module Api
	module V1
		class AccountsController  < ApplicationController
			before_filter :authenticate_user!
			respond_to :json

			def index
				token = params[:auth_token]
				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token and id."}
					return
				end
				@user = User.find_by_authentication_token(token)
				@accounts = Account.where :computer_id => @user.computer_ids

				respond_to do |format|
					format.json { render :json => @accounts}
				end
			end

			def create 

			end 

			def show
				token = params[:auth_token]
				id = params[:id]
				if token.nil? or id.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token and id."}
					return
				end
				@user = User.find_by_authentication_token(token)
				@account = Account.find(id)

				respond_to do |format|
					format.json { render :json => @account }
				end
			end

			def new

			end

			def edit

			end

			def update

			end

			def destroy

			end
		end
	end
end
