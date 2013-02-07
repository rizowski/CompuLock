module Api
	module V1
		class AccountsController  < ApplicationController
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
				@accounts = Account.where computer_id: @user.computer_ids

				respond_to do |format|
					format.json { render :json => @accounts}
				end
			end

			def create 
				token = params[:auth_token]
				account = JSON.parse params[:account]
				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end

				@user = User.find_by_authentication_token(token)

				comp_id = account["computer_id"]
				if comp_id.nil?
					render :status => 400,
						:json => { :message => "The request must contain a 'computer_id'."}
					return
				end

				unless @user.computer_ids.include? comp_id.to_i
					render :status => 401,
						:json => { :message => "The request was declined. Check computer Id."}
					return
				end
				
				if account["user_name"].nil?
					render :status => 400,
						:json => { :message => "'user_name' cannot be null."}
					return
				end

				@account = Account.new(account)
				if @account.save
					respond_to do |format|
						format.json {render json: @account}
					end
				else
					render :status => 400,
						:json => { :message => "Something went wrong with saving the entity."}
					return
				end
			end 

			def show
				token = params[:auth_token]
				id = params[:id]

				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end

				if id.nil?
					render :status => 400,
						:json => { :message => "The request must contain an id."}
					return
				end

				@user = User.find_by_authentication_token(token)
				@accounts = Account.where computer_id: @user.computer_ids

				unless @accounts.pluck(:id).include? id.to_i
					render :status => 401,
						:json => { :message => "Access Denied."}
					return
				end

				@account = Account.find(id)
				respond_to do |format|
					format.json { render :json => @account }
				end
			end

			# this needs to be fixed. http://stackoverflow.com/questions/14704542/recieving-the-appropriate-json-model-rails
			def update
				token = params[:auth_token]
				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end
				respond_with Account.update(params[:id], params[:account])
			end

			def destroy
				token = params[:auth_token]
				id = params[:id]

				if token.nil?
					render :status => 400,
						:json => { :message => "The request must contain an auth token."}
					return
				end
				if id.nil?
					render :status => 400,
						:json => { :message => "The request must contain an id."}
					return
				end

				@user = User.find_by_authentication_token(token)
				@accounts = Account.where computer_id: @user.computer_ids
				
				unless @accounts.pluck(:id).include? id.to_i
					render :status => 401,
						:json => { :message => "Access Denied."}
					return
				end

				if Account.find(id).delete
					respond_to do |format|
						format.json { render :json => {message: "Success"} }
					end
				else
					render :status => 400,
						:json => { :message => "Something went wrong with deleting the entity."}
					return
				end
			end
		end
	end
end
