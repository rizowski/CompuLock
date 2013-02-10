module Api
  module V1
  	class ProcessesController < ApplicationController
  		before_filter :authenticate_user!
		respond_to :json

		def create
			token = params[:auth_token]
			process = JSON.parse params[:process]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			account_id = process["account_id"]
			if account_id.nil?
				render :status => 400,
					:json => { :message => "The request must contain an account id."}
				return
			end

			if process["name"].nil?
				render :status => 400,
					:json => { :message => "The request must contain a name."}
				return
			end

			@user = User.find_by_authentication_token token
			@accounts = Account.where computer_id: @user.computer_ids

			unless @accounts.pluck(:id).include? account_id
				render :status => 401,
					:json => { :message => "The request was declined. Check account_id."}
				return
			end
			@process = AccountProcess.new(process)
			if @process.save
				render json: {process: @process}
			else
				render :status => 400,
					:json => { :message => "There was a problem saving the entity."}
				return
			end
		end

		def show

		end

		def index

		end
  	end
  end
end