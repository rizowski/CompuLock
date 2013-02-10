module Api
  module V1
  	class HistoriesController < ApplicationController
  		before_filter :authenticate_user!
		respond_to :json

		def create
			token = params[:auth_token]
			history = JSON.parse params[:history]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			account_id = history["account_id"]
			if account_id.nil?
				render :status => 400,
					:json => { :message => "The request must contain an account id."}
				return
			end
			if history["domain"].nil?
				render :status => 400,
					:json => { :message => "The request must contain a domain."}
				return
			end
			if history["last_visited"].nil?
				render :status => 400,
					:json => { :message => "The request must contain a last visited date."}
				return
			end

			@user = User.find_by_authentication_token token
			@accounts = Account.where computer_id: @user.computer_ids

			unless @accounts.pluck(:id).include? account_id
				render :status => 401,
					:json => { :message => "The request was declined. Check account_id."}
				return
			end
			@history = AccountHistory.new(history)
			if @history.save
				render json: {history: @history}
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