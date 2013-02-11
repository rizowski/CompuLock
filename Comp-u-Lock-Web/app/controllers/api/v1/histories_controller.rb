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
			token = params[:auth_token]
			history_id = params[:id]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			if history_id.nil? 
				render :status => 400,
					:json => { :message => "The request must contain a history id."}
				return
			end
			@user = User.find_by_authentication_token token
			@history = AccountHistory.find history_id
			@accounts = Account.where computer_id: @user.computer_ids

			if @accounts.pluck(:id).include? @history.account_id
				render json: {history: @history}
			else
				render :status => 401,
					:json => { :message => "The request was declined. Check Account Id."}
				return
			end
		end

		def index
			token = params[:auth_token]
			account_id = params[:account_id]
			if token.nil?
				render :status => 400,
					:json => { :message => "The request must contain an auth token."}
				return
			end
			if account_id.nil?
				render :status => 400,
					:json => { :message => "The request must contain an account id."}
				return
			end
			@accounts = Account.where computer_id: @user.computer_ids
			unless @accounts.pluck(:id).include? account_id
				render :status => 401,
					:json => { :message => "The request was declined."}
				return
			end
			@account = Account.find account_id

			@histories = @account.account_history.all
			render json: {histories: @histories}
		end
  	end
  end
end