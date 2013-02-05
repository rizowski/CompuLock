class Api::V1::AccountsController  < ApplicationController
	respond_to :json
	load_and_authorize_resource
	before_filter :authenticate_user!
	def index
		token = params[:auth_token]
		id = params[:id]
		if token.nil?
			render :status => 400,
				:json => { :message => "The request must contain an auth token."}
			return
		end
		if id.nil?
			render :status => 400,
				:json => { :message => "The request must contain an account id."}
			return
		end
		@user = User.find_by_authentication_token(token)
		@account = Account.find(id)
		if can? :read, @account
			respond_to do |format|
				format.json { render :json => @account }
			end
		else
			render :status => 400,
				:json => { :message => "You are not authorized to view this."}
			return
		end
	end

	def create 

	end 

	def show
		token = params[:auth_token]
		
		if token.nil?
			render :status => 400,
				:json => { :message => "The request must contain an auth token."}
			return
		end
		@user = User.find_by_authentication_token(token)
		@account = Account.find(params[:id])

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